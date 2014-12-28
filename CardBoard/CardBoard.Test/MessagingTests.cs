using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardBoard.Models;
using Newtonsoft.Json.Linq;
using CardBoard.Messages;
using System.Collections.Generic;
using FluentAssertions;

namespace CardBoard.Test
{
    [TestClass]
    public class MessagingTests
    {
        private Application _application;

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application();
        }

        [TestMethod]
        public void CardCreatedOnce()
        {
            JObject body = new JObject();

            _application.ReceiveMessage(new Message
            {
                Type = "CardCreated",
                Predecessors = new List<MessageHash>(),
                Body = body,
                Hash = new MessageHash()
            });

            _application.Board.Cards.Count.Should().Be(1);
        }
    }
}
