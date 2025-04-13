import torch
from torchvision.models.detection import fasterrcnn_resnet50_fpn
from torchvision.models.detection.faster_rcnn import FastRCNNPredictor


class FasterRCNNWrapper(torch.nn.Module):
    def __init__(self, model):
        super(FasterRCNNWrapper, self).__init__()
        self.model = model

    def forward(self, images):
        return self.model(images)  # only images, no targets


def load_model(path, num_classes, device="cpu"):
    model = fasterrcnn_resnet50_fpn(weights=None, pretrained=False)
    in_feats = model.roi_heads.box_predictor.cls_score.in_features
    model.roi_heads.box_predictor = FastRCNNPredictor(in_feats, num_classes)
    checkpoint = torch.load(path, map_location=device)
    model.load_state_dict(checkpoint)
    model.to(device).eval()
    return model


device = "cpu"
num_classes = 6

base_model = load_model("best_faster_rcnn.pth", num_classes, device)
model = FasterRCNNWrapper(base_model)

dummy_input = [torch.randn(3, 640, 640, device=device)]  # Batch of 1 image

torch.onnx.export(
    model,
    dummy_input,
    "model.onnx",
    input_names=['images'],
    output_names=['boxes', 'labels', 'scores'],
    opset_version=11,
    dynamic_axes={'images': {0: 'batch_size'}}
)

print("Exported successfully to model.onnx")

