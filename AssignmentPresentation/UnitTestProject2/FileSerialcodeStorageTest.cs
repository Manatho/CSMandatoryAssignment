using System;
using System.IO;
using NUnit.Framework;
using AssignmentLogic.SerialcodeStorage;

[TestFixture]
public class FileSerialcodeStorageTest
{

    IStoreSerialcodes codeStorage;
    string path;
    int[] codes;
    

    [SetUp]
    public void Init()
    {
        //Creates a temporary folder to store the test files
        path = Directory.GetCurrentDirectory() + "\\SerialTest";
        Directory.CreateDirectory(path);

        codes = new int[100];
        for (int i = 0; i < codes.Length; i++)
            codes[i] = i;

        codeStorage = new FileSerialcodeStorage(path, codes, false);
    }

    [Test]
    public void CheckCodeTest()
    {
        Assert.True(codeStorage.CheckCode(0));
        Assert.True(codeStorage.CheckCode(1));
        Assert.True(codeStorage.CheckCode(99));

        Assert.False(codeStorage.CheckCode(-1));
        Assert.False(codeStorage.CheckCode(100));
        Assert.False(codeStorage.CheckCode(101));
    }

    [Test]
    public void AllCodesTest()
    {
        Assert.That(codes, Is.EquivalentTo(codeStorage.AllCodes()));
    }

    [Test]
    public void SetCodesTest()
    {
        int[] newCodes = new int[] { -1, -2, -3, -4, -5 };
        codeStorage.SetCodes(newCodes);

        Assert.That(newCodes, Is.EquivalentTo(codeStorage.AllCodes()));
    }

    [TearDown]
    public void Dispose()
    {
        //Removes the temporary folder
        Directory.Delete(path, true);
    }
    
}