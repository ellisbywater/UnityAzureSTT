using DiffPlex.DiffBuilder;
using Newtonsoft.Json;
using UnityEngine;

namespace Reader
{
    public static class TextAnalyzer
    {
        public static AnalyzerResult RunDiff(string story, string output)
        {
            AnalyzerResult analyzerResult;
            var diffBuilder = new SideBySideDiffBuilder();
            var diff = diffBuilder.BuildDiffModel(story, output);
            var serializedDiff = JsonConvert.SerializeObject(diff);
            if (diff.OldText.HasDifferences || diff.NewText.HasDifferences)
            {
                analyzerResult = new AnalyzerResult {Success = false, SerializedData = serializedDiff};
            }
            else
            {
                analyzerResult = new AnalyzerResult {Success = true, SerializedData = serializedDiff};
            }
            
            return analyzerResult;
        }
        
    }
    public class AnalyzerResult
    {
        public bool Success { get; set; }
        public string SerializedData { get; set; }
    }
}
