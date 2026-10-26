using System;
using ReSharperPlugin.CleanArchTool.Tests.test.data.Util;

namespace ReSharperPlugin.CleanArchTool.Tests.test.data.Analyzers.ClassTooLong;

public class LongClass
{
    private Guid Id { get; }
    private string FirstName { get; }
    private string LastName { get; }

    private string HealthRecord { get; }

    public LongClass(
        Guid id,
        string firstName,
        string lastName,
        string healthRecord
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        HealthRecord = healthRecord;
    }

    public string GetDisplayName()
    {
        return $"{FirstName}{LastName}";
    }

    public void RunHealthRecordOperation()
    {
        var service = new HealthService();
        service.TestPatient(Id, HealthRecord);
    }

    public string GetInitials()
    {
        return $"{FirstName[0]}{LastName[0]}";
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public bool HasHealthRecord()
    {
        return !string.IsNullOrWhiteSpace(HealthRecord);
    }

    public int GetHealthRecordLength()
    {
        return HealthRecord.Length;
    }

    public bool IsSamePerson(
        Guid otherId
    )
    {
        return Id == otherId;
    }

    public string GetIdAsString()
    {
        return Id.ToString();
    }

    public string GetFirstNameUpper()
    {
        return FirstName.ToUpperInvariant();
    }

    public string GetLastNameUpper()
    {
        return LastName.ToUpperInvariant();
    }

    public void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new InvalidOperationException("Id is empty");
        }
    }

    public void ValidateNames()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            throw new InvalidOperationException("First name is empty");
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            throw new InvalidOperationException("Last name is empty");
        }
    }

    public bool TryRunHealthRecordOperation()
    {
        try
        {
            RunHealthRecordOperation();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}