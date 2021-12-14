using Blockchain.Expert.Enums;

namespace Blockchain.Expert
{
    public static class KnowledgeBase
    {
        public static Tree MockTree =>
            new Tree(KnowledgeType.PatientComplaint, new Knowledge(PatientComplaint.Cough))
            {
                TrueTree = new Tree(KnowledgeType.DoctorInspection, new Knowledge(DoctorInspection.LowOxygen))
                {
                    TrueTree = new Tree(KnowledgeType.LabResult, new Knowledge(LabResult.CovidPositive))
                    {
                        TrueTree = new Tree(KnowledgeType.Diagnosis,
                            new Knowledge(Diagnosis.Covid, "covidprescription"))
                    }
                },
                FalseTree = new Tree(KnowledgeType.PatientComplaint, new Knowledge(PatientComplaint.Headache))
                {
                    TrueTree = new Tree(KnowledgeType.DoctorInspection, new Knowledge(DoctorInspection.BloodPressure))
                    {
                        TrueTree = new Tree(KnowledgeType.LabResult, new Knowledge(LabResult.ThickBlood))
                        {
                            TrueTree = new Tree(KnowledgeType.Diagnosis,
                                new Knowledge(Diagnosis.Hypertension, "hypertensionprescription"))
                        }
                    },
                    FalseTree = new Tree(KnowledgeType.PatientComplaint, new Knowledge(PatientComplaint.Weakness))
                    {
                        TrueTree = new Tree(KnowledgeType.DoctorInspection, new Knowledge(DoctorInspection.Temperature))
                        {
                            TrueTree = new Tree(KnowledgeType.LabResult, new Knowledge(LabResult.WeakImmunity))
                            {
                                TrueTree = new Tree(KnowledgeType.Diagnosis,
                                    new Knowledge(Diagnosis.Flu, "fluprescription"))
                            }
                        }
                    }
                }
            };
    }
}
