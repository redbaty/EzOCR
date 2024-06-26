# EzOCR
A wrapper around [a tesseract wrapper](https://github.com/charlesw/tesseract), which aims to improve easy of use.

Features:

* Automatically downloads language file from tessdata.
* Makes string OCR interface easier to use.
* Stream to Pix converter.

# Usage

#### Example using a bitmap as input.

```
var pix = PixConverter.ToPix(bitmap);
var ocrText = await pix.GetTextAndEnsureData()
                       .CleanAndFlattenString();
```

---

#### Example using a stream as input.

```
var pix = imageStream.ConvertToPix();
var ocrText = await pix.GetTextAndEnsureData()
                       .CleanAndFlattenString();
```

# Linux

To run in linux you need to install some dependencies, [here's a great comment/thread exaplaining how to do it](https://github.com/charlesw/tesseract/issues/503#issuecomment-1863208371)
