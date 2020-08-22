namespace InsuranceAppWebAPI.Models
{
    public class PolicyRule
    {
        private readonly int _coverage;
        private readonly RiskType _riskType;
        public PolicyRule(int coverage, RiskType riskType)
        {
            _coverage = coverage;
            _riskType = riskType;
        }

        public string Validate()
        {
            if (_riskType == RiskType.High)
            {
               if(_coverage>50)
                {
                    return "Coverage should be less than 50%";
                }
            }
            return null;
        }
    }
}
