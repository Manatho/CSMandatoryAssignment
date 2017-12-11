using System;
using System.IO;
using NUnit.Framework;
using AssignmentLogic;

namespace AssignmentLogicTests
{
    [TestFixture]
    public class GiveAwayControllerTest
    {
        IGiveAway controller;
        string path;
        Submission sub1, dub1, weirdEmail, incorrectNumber, missingFirstName, missingLastName, sub6, sub7;

        [SetUp]
        public void Init()
        {
            //Creates a temporary folder for testing
            path = Directory.GetCurrentDirectory() + "\\ControllerTest";
            Directory.CreateDirectory(path);

            sub1 = new Submission("1", "a", "1a@Test.com", "73577357", DateTime.Now, 1);
            dub1 = new Submission("11", "aa", "11a@Test.com", "73577357", DateTime.Now, 1);
            weirdEmail = new Submission("2", "b", "2asd", "73577357", DateTime.Now, 2);
            incorrectNumber = new Submission("3", "c", "3c@Test.com", "asdasd", DateTime.Now, 3);
            missingFirstName = new Submission("", "d", "4d@Test.com", "73577357", DateTime.Now, 4);
            missingLastName = new Submission("5", "", "5e@Test.com", "73577357", DateTime.Now, 5);
            sub6 = new Submission("1", "a", "1a@Test.com", "73577357", DateTime.Now, 6);
            sub7 = new Submission("1", "a", "1a@Test.com", "73577357", DateTime.Now, 7);

            controller = new GiveAwayController(path);
        }

        [Test]
        public void SubmitTest()
        {
            Assert.AreEqual(SubmitStates.Success, controller.Submit(sub1));
            var AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1}));

            //Test that two submission with the same code cannot be added
            Assert.AreEqual(SubmitStates.Duplicate, controller.Submit(dub1));
            AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1 }));

            //Tests email (More test confirming the whole range of mails would be appropriate, same goes for later test)
            Assert.AreEqual(SubmitStates.InvalidInformation, controller.Submit(weirdEmail));
            AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1 }));

            //Tests phonenumber
            Assert.AreEqual(SubmitStates.InvalidInformation, controller.Submit(incorrectNumber));
            AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1 }));

            //Tests missing firstname
            Assert.AreEqual(SubmitStates.InvalidInformation, controller.Submit(missingFirstName));
            AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1 }));

            //Tests missing surname
            Assert.AreEqual(SubmitStates.InvalidInformation, controller.Submit(missingLastName));
            AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1 }));
        }

        [Test]
        public void AllSubmissionsTest()
        {
            controller.Submit(sub1);
            controller.Submit(sub6);
            controller.Submit(sub7);
            var AllSubmissions = controller.AllSubmissions();
            Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1, sub6, sub7 }));
        }

        [Test]
        public void DrawWinnerTest()
        {
            controller.Submit(sub1);
            controller.Submit(sub6);
            controller.Submit(sub7);
            Assert.That(new Submission[] { sub1, sub6, sub7 }, Has.Member(controller.DrawWinner()));
        }


        [TearDown]
        public void Dispose()
        {
            //Deletes the temporary folder
            Directory.Delete(path, true);
        }
    }
    
}
