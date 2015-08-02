using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoverMob;
using RoverMob.Messaging;
using System.Threading.Tasks;
using FieldService.Bridge.Mapping;
using FieldService.Bridge.Utility;
using System.Data.Common;
using FieldService.Bridge.Identity;

namespace FieldService.Bridge.Scanning
{
    public class FieldServiceScanner : Scanner
    {
        private FileMessageQueue _queue;
        private HttpMessagePump _pump;

        private MessageIdMap _messageIdMap;
        private IdentityServiceProxy _identityService;
        
        public FieldServiceScanner(string queueFolderName, Uri distributorUri, Uri identityUri)
        {
            _queue = new FileMessageQueue(queueFolderName);
            _pump = new HttpMessagePump(distributorUri, _queue, new NoOpBookmarkStore());
            _pump.ApiKey = "123456";

            _messageIdMap = new MessageIdMap();
            _identityService = new IdentityServiceProxy(identityUri);
            _identityService.ApiKey = "123456";

            AddTableScanner("Home", r => HomeRecord.FromDataRow(r),
                OnInsertHome, OnUpdateHome);
            AddTableScanner("Incident", r => IncidentRecord.FromDataRow(r),
                OnInsertIncident, OnUpdateIncident);
            AddTableScanner("Visit", r => VisitRecord.FromDataRow(r),
                OnInsertVisit);
        }

        protected override async Task StartUp()
        {
            var messages = await _queue.LoadAsync();
            _pump.SendAllMessages(messages);
        }

        private void EmitMessage(Message message)
        {
            _queue.Enqueue(message);
            _pump.Enqueue(message);
        }

        private async Task OnInsertHome(HomeRecord record, DbConnection connection)
        {
            Console.WriteLine("Inserted home #{0} with address {1}.",
                record.HomeId, record.Address);

            var homeId = await _messageIdMap.GetOrCreateObjectId(
                "Home", record.HomeId);

            EmitMessage(Message.CreateMessage(
                string.Empty,
                "Home",
                Guid.Empty,
                new
                {
                    HomeId = homeId
                }));
            Message message = Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                homeId,
                new
                {
                    Value = record.Address
                });
            await _messageIdMap.SaveMessageHash(
                "Home", "Address", record.HomeId, record.Address,
                message.Hash);
            EmitMessage(message);
        }

        private async Task OnUpdateHome(
            HomeRecord oldValues, HomeRecord newValues, DbConnection connection)
        {
            Console.WriteLine("Updated home #{0} from address {1} to address {2}.",
                newValues.HomeId, oldValues.Address, newValues.Address);

            var homeId = await _messageIdMap.GetOrCreateObjectId(
                "Home", newValues.HomeId);

            var priorMessageHash = await _messageIdMap.GetMessageHash(
                "Home", "Address", oldValues.HomeId, oldValues.Address);

            Message message = Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                Predecessors.Set
                    .In("prior", priorMessageHash),
                homeId,
                new
                {
                    Value = newValues.Address
                });
            await _messageIdMap.SaveMessageHash(
                "Home", "Address", newValues.HomeId, newValues.Address,
                message.Hash);
            EmitMessage(message);
        }

        private async Task OnInsertIncident(IncidentRecord record, DbConnection connection)
        {
            Console.WriteLine("Inserted incident #{0} with description {1}.",
                record.IncidentId, record.Description);

            var homeId = await _messageIdMap.GetOrCreateObjectId(
                "Home", record.HomeId);
            var incidentId = await _messageIdMap.GetOrCreateObjectId(
                "Incident", record.IncidentId);

            EmitMessage(Message.CreateMessage(
                string.Empty,
                "Incident",
                Guid.Empty,
                new
                {
                    IncidentId = incidentId,
                    HomeId = homeId
                }));
            Message message = Message.CreateMessage(
                incidentId.ToCanonicalString(),
                "IncidentDescription",
                incidentId,
                new
                {
                    Value = record.Description
                });
            await _messageIdMap.SaveMessageHash(
                "Incident", "Description", record.IncidentId, record.Description,
                message.Hash);
            EmitMessage(message);
        }

        private async Task OnUpdateIncident(
            IncidentRecord oldValues, IncidentRecord newValues,
            DbConnection connection)
        {
            Console.WriteLine("Updated incident #{0} from description {1} to {2}.",
                newValues.IncidentId, oldValues.Description, newValues.Description);

            var incidentId = await _messageIdMap.GetOrCreateObjectId(
                "Incident", newValues.IncidentId);

            var priorMessageHash = await _messageIdMap.GetMessageHash(
                "Incident", "Description", oldValues.IncidentId, oldValues.Description);
            Message message = Message.CreateMessage(
                incidentId.ToCanonicalString(),
                "IncidentDescription",
                Predecessors.Set
                    .In("prior", priorMessageHash),
                incidentId,
                new
                {
                    Value = newValues.Description
                });
            await _messageIdMap.SaveMessageHash(
                "Incident", "Description", newValues.IncidentId, newValues.Description,
                message.Hash);
            EmitMessage(message);
        }

        private async Task OnInsertVisit(VisitRecord record, DbConnection connection)
        {
            Console.WriteLine("Inserted visit #{0} assigning technician #{1} to incident #{2}.",
                record.VisitId, record.TechnicianId, record.IncidentId);

            int homeFk = await connection.ExecuteScalarAsync<int>(
                "select HomeId from Incident where IncidentId = @p1",
                record.IncidentId);
            string technicianLogin = await connection.ExecuteScalarAsync<string>(
                "select Login from Technician where TechnicianId = @p1",
                record.TechnicianId);

            Guid technicianId = await _identityService.GetTechnicianIdentifier(
                technicianLogin);
            Guid visitId    = await _messageIdMap.GetOrCreateObjectId(
                "Visit",        record.VisitId);
            Guid incidentId = await _messageIdMap.GetOrCreateObjectId(
                "Incident",     record.IncidentId);
            Guid homeId     = await _messageIdMap.GetOrCreateObjectId(
                "Home",         homeFk);
            DateTime startTime = record.StartTime;
            DateTime endTime   = record.EndTime;

            var message = Message.CreateMessage(
                new TopicSet()
                    .Add(technicianId.ToCanonicalString())
                    .Add(incidentId.ToCanonicalString()),
                "Visit",
                Predecessors.Set,
                technicianId,
                new
            {
                IncidentId = incidentId,
                VisitId = visitId,
                HomeId = homeId,
                StartTime = startTime,
                EndTime = endTime
            });
            EmitMessage(message);
        }
    }
}
