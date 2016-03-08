namespace opm_validation_service.Models
{
    public class OpmVerificationResult
    {
        public bool Result { get; set; }

        //Only for (de)serialization purpuse
        public OpmVerificationResult()
        {
        }

        public OpmVerificationResult(bool result)
        {
            Result = result;
        }
    }
}