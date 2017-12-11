using System;
using System.IO;
using NUnit.Framework;
using AssignmentLogic.SubmissionStorage;
using AssignmentLogic;

[TestFixture]
public class FileSubmissionStorageTest
{

    IStoreSubmissions submissionStorage;
    string path;

    Submission sub1, sub2, sub3, dub1;


    [SetUp]
    public void Init()
    {
        path = Directory.GetCurrentDirectory() + "\\SubmissionTest";
        Directory.CreateDirectory(path);


        sub1 = new Submission("1", "a", "1a@Test.com", "7357-7357", DateTime.Now, 1);
        sub2 = new Submission("2", "b", "2b@Test.com", "7357-7357", DateTime.Now, 2);
        sub3 = new Submission("3", "c", "3c@Test.com", "7357-7357", DateTime.Now, 3);
        dub1 = new Submission("4", "d", "4d@Test.com", "7357-7357", DateTime.Now, 1);

        submissionStorage = new FileSubmissionStorage(path);
    }

    [Test]
    public void StoreAndFindAllTest()
    {
        Assert.AreEqual(SubmitStates.Success, submissionStorage.Store(sub1, sub2, sub3));
        var AllSubmissions = submissionStorage.FindAll();
        Assert.That(AllSubmissions, Is.EquivalentTo(new Submission[] { sub1, sub2, sub3 }));
    }

    [Test]
    public void StoreDuplicatesTest()
    {
        //Store with different submissions with same serisal
        Assert.AreEqual(SubmitStates.Duplicate, submissionStorage.Store(sub1, dub1));
        var allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[0]));

        //Store with several params with some dublicates
        Assert.AreEqual(SubmitStates.Duplicate, submissionStorage.Store(sub1, sub2, sub3, sub1, sub2, sub3, dub1));
        allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[0]));

        //Store with single elements
        Assert.AreEqual(SubmitStates.Success, submissionStorage.Store(sub1));
        Assert.AreEqual(SubmitStates.Success, submissionStorage.Store(sub2));
        Assert.AreEqual(SubmitStates.Duplicate, submissionStorage.Store(sub1));
        allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] { sub1, sub2 }));

        //Store with several params with no dublicates, but with existing elements in submissionStorage
        Assert.AreEqual(SubmitStates.Duplicate, submissionStorage.Store(sub1, sub3));
        allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] { sub1, sub2 }));

        //Store with another element with existing Serialcode.
        Assert.AreEqual(SubmitStates.Duplicate, submissionStorage.Store(dub1));
        allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] { sub1, sub2 }));
    }

    [Test]
    public void RemoveTest()
    {
        submissionStorage.Store(sub1, sub2, sub3);

        //Remove 1 element
        Assert.True(submissionStorage.Remove(sub1.SerialNumber));
        var allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] { sub2, sub3 }));

        //Remove non-existent element
        Assert.False(submissionStorage.Remove(sub1.SerialNumber));
        allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] { sub2, sub3 }));

        //Remove all elements
        Assert.True(submissionStorage.Remove(sub2.SerialNumber));
        Assert.True(submissionStorage.Remove(sub3.SerialNumber));
        allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] {}));

    }

    [Test]
    public void RemoveAllTest()
    {
        submissionStorage.Store(sub1, sub2, sub3);

        submissionStorage.RemoveAll();
        var allSubmissions = submissionStorage.FindAll();
        Assert.That(allSubmissions, Is.EquivalentTo(new Submission[] { }));

    }

    [Test]
    public void FindTest()
    {
        submissionStorage.Store(sub1, sub2, sub3);
        Assert.AreEqual(sub1, submissionStorage.Find(sub1.SerialNumber));
        Assert.AreEqual(sub3, submissionStorage.Find(sub3.SerialNumber));
        Assert.AreEqual(null, submissionStorage.Find(190584));

    }


    [TearDown]
    public void Dispose()
    {
        Directory.Delete(path, true);
    }

}