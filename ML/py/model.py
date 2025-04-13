import torch
from torchvision.models.detection import fasterrcnn_resnet50_fpn
from torchvision.models.detection.faster_rcnn import FastRCNNPredictor
from torchvision.transforms import functional as F
from PIL import Image
import matplotlib.pyplot as plt
import matplotlib.patches as patches

# 🔁 Якщо потрібна твоя мапа id -> label
id2label = {
    1: 'pavement',
    2: 'ramp',
    3: 'step',
    4: 'stair',
    5: 'grab_bar'
}

# ✅ Завантаження моделі
from torchvision.models.detection import fasterrcnn_resnet50_fpn, FasterRCNN_ResNet50_FPN_Weights
from torchvision.models.detection.faster_rcnn import FastRCNNPredictor

def load_model(path, num_classes, device="cpu"):
    # 1) беремо претреновану на COCO модель
    weights = FasterRCNN_ResNet50_FPN_Weights.DEFAULT
    model = fasterrcnn_resnet50_fpn(weights=weights, pretrained=True)
    # 2) міняємо голову під свій num_classes
    in_feats = model.roi_heads.box_predictor.cls_score.in_features
    model.roi_heads.box_predictor = FastRCNNPredictor(in_feats, num_classes)
    # 3) підвантажуємо свої ваги
    # checkpoint = torch.load(path, map_location=device)
    # # якщо ти зберігав повний state_dict:
    # model.load_state_dict(checkpoint)
    # або, якщо ти зберігав чекпоінт із ключем 'model_state_dict':
    # model.load_state_dict(checkpoint['model_state_dict'])
    model.to(device).eval()
    return model



def predict_labels_with_scores(model, image_path, id2label, score_threshold=0.0, device="cpu"):
    """
    Повертає dict: {label_name: max_score} для всіх класів із id2label.
    Якщо клас не було виявлено з score >= score_threshold, його значення буде 0.0.
    """
    # 1. Ініціалізація результату нулями для всіх лейблів
    result = {name: 0.0 for name in id2label.values()}

    # 2. Завантажуємо й перетворюємо зображення
    image = Image.open(image_path).convert("RGB")
    image_tensor = F.to_tensor(image).to(device)

    # 3. Інференс
    model.to(device).eval()
    with torch.no_grad():
        output = model([image_tensor])[0]

    # 4. Проходимо по передбаченнях
    labels = output['labels'].cpu()
    scores = output['scores'].cpu()

    for lbl, sc in zip(labels, scores):
        sc_val = sc.item()
        if sc_val < score_threshold:
            continue
        name = id2label.get(lbl.item(), str(lbl.item()))
        # зберігаємо лише максимальний score для кожного label
        if sc_val > result[name]:
            result[name] = sc_val

    return result


def analyze_image(model, image_path, score_threshold=0.2):
    image = Image.open(image_path).convert("RGB")
    image_tensor = F.to_tensor(image)

    with torch.no_grad():
        output = model([image_tensor])[0]

    # Фільтрація по score
    boxes = output['boxes']
    labels = output['labels']
    scores = output['scores']

    fig, ax = plt.subplots(1)
    ax.imshow(image)

    for box, label, score in zip(boxes, labels, scores):
        if score >= score_threshold:
            xmin, ymin, xmax, ymax = box
            rect = patches.Rectangle((xmin, ymin), xmax - xmin, ymax - ymin,
                                     linewidth=2, edgecolor='lime', facecolor='none')
            ax.add_patch(rect)
            label_text = f"{id2label.get(label.item(), label.item())} ({score:.2f})"
            ax.text(xmin, ymin - 5, label_text, color='lime', fontsize=9, backgroundcolor='black')

    plt.axis('off')
    plt.show()


id2label = {1:'pavement', 2:'ramp', 3:'step', 4:'stair', 5:'grab_bar'}

# 🔁 Використання
model = load_model("Accessibility Barriers/code/checkpoint4.pth", num_classes=6)  # 5 класів + background
print(model.transform)

analyze_image(model, "Accessibility Barriers/code/images.jpg")
labels_scores = predict_labels_with_scores(model, "Accessibility Barriers/code/images.jpg", id2label, score_threshold=0.3)


print(labels_scores)

import os

print(os.getcwd())  # Додано для перевірки поточної директорії