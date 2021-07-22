// '2021-07-22 / B.Agullo / 
// this script creates a hex to numeric calculated table required for Color Light and Color Transparency Calculation groups

string calcTableName = "Hexadecimal to Number"; 
string hexCodeColumnName = "Hex Value"; 
string numberColumnName = "Numeric Value"; 
string percentOpacityColumnName = "Percent Opacity"; 
string percentTransparencyColumnName = "Percent Transparency"; 


string transparentMeasureName = "Transparency Suffix"; 
//
// ----- do not modify script below this line -----
//

if (Model.Tables.Contains(calcTableName)) {
    Error(calcTableName + " already exists in the model.");
    return;
}; 

string calcTableExpression = 
    "VAR numericValueColumn =" + 
    "\n    SELECTCOLUMNS ( GENERATESERIES ( 0, 255, 1 ), \"" + numberColumnName + "\", [Value] )" + 
    "\nVAR result =" + 
    "\n    GENERATE (" + 
    "\n        numericValueColumn," + 
    "\n        VAR numericValue = [Numeric Value]" + 
    "\n        VAR firstHexNum =" + 
    "\n            INT ( DIVIDE ( numericValue, 16 ) )" + 
    "\n        VAR firstHexLetter =" + 
    "\n            IF (" + 
    "\n                firstHexNum <= 9," + 
    "\n                FORMAT ( firstHexNum, \"0\" )," + 
    "\n                UNICHAR ( 65 + ( firstHexNum - 10 ) )" + 
    "\n            )" + 
    "\n        VAR secondHexNum =" + 
    "\n            INT ( ( DIVIDE ( numericValue, 16 ) - firstHexNum ) * 16 )" + 
    "\n        VAR secondHexLetter =" + 
    "\n            IF (" + 
    "\n                secondHexNum <= 9," + 
    "\n                FORMAT ( secondHexNum, \"0\" )," + 
    "\n                UNICHAR ( 65 + ( secondHexNum - 10 ) )" + 
    "\n            )" + 
    "\n        RETURN" + 
    "\n            ROW (" + 
    "\n                \"" + hexCodeColumnName + "\", firstHexLetter & secondHexLetter," + 
    "\n                \"" + percentOpacityColumnName + "\", numericValue / 255, " + 
    "\n                \"" + percentTransparencyColumnName + "\", 1-numericValue / 255" + 
    "\n            )" + 
    "\n    )" + 
    "\nRETURN" + 
    "\n    result";

    
var transparencyTable =  Model.AddCalculatedTable(calcTableName,calcTableExpression);
transparencyTable.FormatDax(); 
transparencyTable.Description = "Lookup table to translate numbers to hexadecimal";
transparencyTable.IsHidden = true;

string transparentMeasureExpression = "FORMAT(SELECTEDVALUE('" + calcTableName +"'[Hex Value],\"FF\"),\"@\")";

var transparentMeasure = transparencyTable.AddMeasure(transparentMeasureName,transparentMeasureExpression);
transparentMeasure.FormatDax(); 
transparentMeasure.Description = "Applend to any 6 digit hex value to get a color with the transparency selected in " + calcTableName;
    
    