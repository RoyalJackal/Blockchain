using System;
using System.Collections.Generic;
using Blockchain.Expert.Enums;

namespace Blockchain.Expert
{
    public class Tree
    {
        public KnowledgeType KnowledgeType { get; }

        public Knowledge Knowledge { get; }

        public Tree TrueTree { get; set; }

        public Tree FalseTree { get; set; }

        public Tree(KnowledgeType knowledgeType, Knowledge knowledge)
        {
            KnowledgeType = knowledgeType;
            Knowledge = knowledge;
        }

        public Knowledge TraverseTree(List<PatientComplaint> complaints, List<DoctorInspection> inspectionResults,
            List<LabResult> labResults)
        {
            switch (KnowledgeType)
            {
                case KnowledgeType.LabResult:
                    if (labResults.Contains(Knowledge.LabResult.Value))
                    {
                        Console.Write($"because lab result showed {Knowledge.LabResult.Value}, ");
                        if (TrueTree == null)
                        {
                            Console.WriteLine("system couldn't find a correct diagnosis.");
                            return null;
                        }
                        return TrueTree.TraverseTree(complaints, inspectionResults, labResults);
                    }
                    else
                    {
                        Console.Write($"because lab result showed no {Knowledge.LabResult.Value}, ");
                        if (FalseTree == null)
                        {
                            Console.WriteLine("system couldn't find a correct diagnosis.");
                            return null;
                        }
                        return FalseTree.TraverseTree(complaints, inspectionResults, labResults);
                    }
                case KnowledgeType.DoctorInspection:
                    if (inspectionResults.Contains(Knowledge.DoctorInspection.Value))
                    {
                        Console.Write($"because doctor inspection showed {Knowledge.DoctorInspection.Value}, ");
                        if (TrueTree == null)
                        {
                            Console.WriteLine("system couldn't find a correct diagnosis.");
                            return null;
                        }
                        return TrueTree.TraverseTree(complaints, inspectionResults, labResults);
                    }
                    else
                    {
                        Console.Write($"because doctor inspection showed no {Knowledge.DoctorInspection.Value}, ");
                        if (FalseTree == null)
                        {
                            Console.WriteLine("system couldn't find a correct diagnosis.");
                            return null;
                        }
                        return FalseTree.TraverseTree(complaints, inspectionResults, labResults);
                    }
                case KnowledgeType.PatientComplaint:
                    if (complaints.Contains(Knowledge.PatientComplaint.Value))
                    {
                        Console.Write($"because the patient had a complaint about {Knowledge.PatientComplaint.Value}, ");
                        if (TrueTree == null)
                        {
                            Console.WriteLine("system couldn't find a correct diagnosis.");
                            return null;
                        }
                        return TrueTree.TraverseTree(complaints, inspectionResults, labResults);
                    }
                    else
                    {
                        Console.Write($"because the patient had no complaint about {Knowledge.PatientComplaint.Value}, ");
                        if (FalseTree == null)
                        {
                            Console.WriteLine("system couldn't find a correct diagnosis.");
                            return null;
                        }
                        return FalseTree.TraverseTree(complaints, inspectionResults, labResults);
                    }
                case KnowledgeType.Diagnosis:
                    Console.WriteLine($"system found out that correct diagnosis was {Knowledge.Diagnosis.Value}");
                    return Knowledge;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
