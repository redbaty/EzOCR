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

###### :warning: Warning! This method writes to a temporary file.

```
var pix = imageStream.ConvertToPix();
var ocrText = await pix.GetTextAndEnsureData()
                       .CleanAndFlattenString();
```
