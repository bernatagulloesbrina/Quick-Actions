// '2021-07-22 / B.Agullo / 
// this script creates a calculation group that modifies the light balance of a color measure affected by it

string calculationGroupName = "Light Effect"; 
string calculationGroupColumnName = "Light Effect"; 

//this name must match the table name already present in the model 
string calcTableName = "Hexadecimal to Number"; 
string hexCodeColumnName = "Hex Value"; 
string numberColumnName = "Numeric Value"; 


int[] lighterLevels = {60,40,20}; //write descending
int[] darkerLevels = {25,50}; //write ascending 

//affected measures table should contain ONLY the names of the color measures to be affected by the calc group
//sucha a table can be created with the "Affected Measures Calc Table" macro
string affectedMeasuresTableName = "Affected Measures"; 
string affectedMeasuresColumnName = "Measure";



// ----- do not modify beyond this line if you do not know what you are doing 



string hexCodeQualified = "'" + calcTableName + "'[" + hexCodeColumnName + "]"; 
string numberQualified = "'" + calcTableName + "'[" + numberColumnName + "]";
int calcItemOrdinalIndex = 0; 

Model.DiscourageImplicitMeasures = true;

var calculationGroupTable1 = Model.AddCalculationGroup(calculationGroupName);

(Model.Tables[calculationGroupName].Columns["Name"] as DataColumn).Name = calculationGroupColumnName;

//prepares part of the expressions that come 
string affectedMeasuresValues = "VALUES('" + affectedMeasuresTableName + "'[" + affectedMeasuresColumnName + "])";

//wrapping statement to avoid affecting all measures in the scope 
string calcItemProtection = 
    "IF(" + 
    "   SELECTEDMEASURENAME() IN " + affectedMeasuresValues + "," + 
    "   <CODE> ," + 
    "   SELECTEDMEASURE() " + 
    ")";

var calcItemExpression = 
"VAR hexValue =" + 
"\n    SELECTEDMEASURE()" + 
"\nVAR calcItemName =" + 
"\n    SELECTEDVALUE( '" + calculationGroupName + "'[Light Effect], \"00% lighter\" )" + 
"\nVAR hexValueOk =" + 
"\n    LEN( hexvalue ) = 7" + 
"\n        && LEFT( hexValue, 1 ) = \"#\"" + 
"\nRETURN" + 
"\n    IF(" + 
"\n        hexValueOk," + 
"\n        VAR hexRed =" + 
"\n            MID( hexValue, 2, 2 )" + 
"\n        VAR hexGreen =" + 
"\n            MID( hexValue, 4, 2 )" + 
"\n        VAR hexBlue =" + 
"\n            MID( hexValue, 6, 2 )" + 
"\n        VAR numRed =" + 
"\n            LOOKUPVALUE( " + numberQualified + ", " + hexCodeQualified + ", hexRed )" + 
"\n        VAR numGreen =" + 
"\n            LOOKUPVALUE(" + 
"\n                " + numberQualified + "," + 
"\n                " + hexCodeQualified + ", hexGreen" + 
"\n            )" + 
"\n        VAR numBlue =" + 
"\n            LOOKUPVALUE(" + 
"\n                " + numberQualified + "," + 
"\n                " + hexCodeQualified + ", hexBlue" + 
"\n            )" + 
"\n        VAR pct =" + 
"\n            VALUE( LEFT( calcItemName, FIND( \"%\", calcItemName, 1, - 1 ) - 1 ) )" + 
"\n        VAR pctSign =" + 
"\n            IF( RIGHT( calcItemName, 6 ) = \"darker\", - 1, 1 )" + 
"\n        VAR q = 1 + pctSign * ( pct / 100 )" + 
"\n        VAR newNumRed =" + 
"\n            IF( q < 1, ROUNDUP( numRed * q, 0 ), ROUNDUP( numRed + ( 255 - numred ) * ( q - 1 ) ,0) )" + 
"\n        VAR newNumGreen =" + 
"\n            IF( q < 1, ROUNDUP( numGreen * q, 0 ), ROUNDUP( numGreen + ( 255 - numGreen ) * ( q - 1 ) ,0) )" + 
"\n            " + 
"\n        VAR newNumBlue =" + 
"\n            IF( q < 1, ROUNDUP( numBlue * q, 0 ), ROUNDUP( numBlue + ( 255 - numBlue ) * ( q - 1 ) ,0) )" + 
"\n        " + 
"\n        VAR newHexRed =" + 
"\n            LOOKUPVALUE(" + 
"\n                " + hexCodeQualified + "," + 
"\n                " + numberQualified + ", newNumRed" + 
"\n            )" + 
"\n        VAR newHexGreen =" + 
"\n            LOOKUPVALUE(" + 
"\n                " + hexCodeQualified + "," + 
"\n                " + numberQualified + ", newNumGreen" + 
"\n            )" + 
"\n        VAR newHexBlue =" + 
"\n            LOOKUPVALUE(" + 
"\n                " + hexCodeQualified + "," + 
"\n                " + numberQualified + ", newNumBlue" + 
"\n            )" + 
"\n        VAR newHexValue = \"#\" & newHexRed & newHexGreen & newHexBlue" + 
"\n        RETURN" + 
"\n            newHexValue," + 
"\n        \"\"" + 
"\n    )"; 


foreach(var lightLevel in lighterLevels){

    string calcItemName = lightLevel  + "% lighter"; 
    var lightCalcItem = calculationGroupTable1.AddCalculationItem(calcItemName);
    lightCalcItem.Expression = calcItemProtection.Replace("<CODE>",calcItemExpression);
    lightCalcItem.FormatDax(); 
    lightCalcItem.Ordinal = calcItemOrdinalIndex; 
    calcItemOrdinalIndex++;     
};


var noChangeItem = calculationGroupTable1.AddCalculationItem("No Change","SELECTEDMEASURE()");
calcItemOrdinalIndex++;  

foreach(var darkLevel in darkerLevels){

    string calcItemName = darkLevel  + "% darker"; 
    var darkCalcItem = calculationGroupTable1.AddCalculationItem(calcItemName);
    darkCalcItem.Expression = calcItemExpression; 
    darkCalcItem.FormatDax(); 
    calcItemOrdinalIndex++;  

};


