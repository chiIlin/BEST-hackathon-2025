using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DetectionBox
{
    public float X1, Y1, X2, Y2;
    public float Score;
    public int Label;
    public string LabelName;
    public bool IsInclusive;
}

public class DetectionResult
{
    public List<DetectionBox> Boxes { get; set; } = new();
}

public class FasterRCNNDetector
{
    private readonly InferenceSession _session;

    private static readonly Dictionary<int, (string Label, bool IsInclusive)> LabelMap = new()
    {
        {1, ("pavement", true)},
        {2, ("ramp", true)},
        {3, ("step", false)},
        {4, ("stair", false)},
        {5, ("grab_bar", true)}
    };

    public FasterRCNNDetector(string modelPath)
    {
        _session = new InferenceSession(modelPath);
    }

    public DetectionResult Predict(string imagePath, string outputName = null)
    {
        var mat = Cv2.ImRead(imagePath);
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2RGB);
        Cv2.Resize(mat, mat, new OpenCvSharp.Size(640, 640));
        mat.ConvertTo(mat, MatType.CV_32FC3, 1.0 / 255.0);

        var input = new DenseTensor<float>(new[] { 3, 640, 640 });

        for (int y = 0; y < 640; y++)
        {
            for (int x = 0; x < 640; x++)
            {
                var color = mat.At<Vec3f>(y, x);
                input[0, y, x] = color.Item0;
                input[1, y, x] = color.Item1;
                input[2, y, x] = color.Item2;
            }
        }

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("images", input)
        };

        using var results = _session.Run(inputs);

        var boxesTensor = results.First(x => x.Name == "boxes").AsTensor<float>();
        var labelsTensor = results.First(x => x.Name == "labels").AsTensor<int>();
        var scoresTensor = results.First(x => x.Name == "scores").AsTensor<float>();

        if (boxesTensor == null || labelsTensor == null || scoresTensor == null)
            throw new Exception("Помилка при обробці моделі: результат пустий");

        var boxes = boxesTensor.ToArray();
        var labels = labelsTensor.ToArray();
        var scores = scoresTensor.ToArray();

        var output = new DetectionResult();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < 0.3f) continue;

            var (labelName, isInclusive) = LabelMap.ContainsKey(labels[i])
                ? LabelMap[labels[i]]
                : ("unknown", false);

            output.Boxes.Add(new DetectionBox
            {
                X1 = boxes[i * 4 + 0],
                Y1 = boxes[i * 4 + 1],
                X2 = boxes[i * 4 + 2],
                Y2 = boxes[i * 4 + 3],
                Score = scores[i],
                Label = labels[i],
                LabelName = labelName,
                IsInclusive = isInclusive
            });
        }

        return output;
    }
}
