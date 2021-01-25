namespace Ezley.Domain.ContractManagement
{
    public enum ContractStatus
    {
        Draft = 10,      // Being drafted
        Approved = 20,   // Draft is approved by approver
        Cancelled = 30,  // not approved and won't be signed
        Signed = 40,     // accepted by promisee
        Activated = 50,  // accepted by both parties
    }

    public enum SignatureStatus
    {
        NotSigned = 10,
        OutForSignature = 20,
        ManuallySigned = 30,
        ESigned = 40,
        Declined = 50,
    }

    public enum ContractStage
    {
        NotSigned = 10, // Not signed yet
        Active = 20, // Signed and started in relation to Active date
        UpForRenewal = 30, // within 1 month of notice period, if notice period is 3 months, then (1 + 3) this start 4 months before end date
        NotRenewed = 40, // still active, but passed renewal period so it is in Notice Period
        NoticeServed = 50, // notice served that contract will not be renewed and will terminate. Must manually set this stage.
        Expired = 60, // Contract expired
        Terminated = 70, // Contract terminated and reason for termination recorded.
    }
}