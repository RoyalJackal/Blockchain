using Blockchain.Expert.Enums;

namespace Blockchain.Expert
{
    public class Knowledge
    {
        public LabResult? LabResult { get; }

        public DoctorInspection? DoctorInspection { get; }

        public PatientComplaint? PatientComplaint { get; }

        public Diagnosis? Diagnosis { get; }

        public string Prescription { get; }

        public Knowledge(LabResult labResult)
        {
            LabResult = labResult;
            DoctorInspection = null;
            PatientComplaint = null;
            Diagnosis = null;
        }

        public Knowledge(DoctorInspection doctorInspection)
        {
            LabResult = null;
            DoctorInspection = doctorInspection;
            PatientComplaint = null;
            Diagnosis = null;
        }

        public Knowledge(PatientComplaint patientComplaint)
        {
            LabResult = null;
            DoctorInspection = null;
            PatientComplaint = patientComplaint;
            Diagnosis = null;
        }

        public Knowledge(Diagnosis diagnosis, string prescription)
        {
            LabResult = null;
            DoctorInspection = null;
            PatientComplaint = null;
            Diagnosis = diagnosis;
            Prescription = prescription;
        }
    }
}
