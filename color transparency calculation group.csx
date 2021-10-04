// '2021-07-26 / B.Agullo / 
// this script creates a calculation group that modifies the transparency of a color measure affected by it

string calculationGroupName = "Transparency Effect"; 
string calculationGroupColumnName = "Transparency Effect"; 

//this name must match the table name already present in the model (use hex to number table script to create it)
string calcTableName = "Hexadecimal to Number"; 
string hexCodeColumnName = "Hex Value"; 
string transparencyColumnName = "Percent Transparency"; 

int[] transparencyLevels = {20,40,50,60,80,100}; 

//affected measures table should contain ONLY the names of the color measures to be affected by the calc group
//sucha a table can be created with the "Affected Measures Calc Table" macro
string affectedMeasuresTableName = "Affected Measures"; 
string affectedMeasuresColumnName = "Measure";

// ----- do not modify beyond this line if you do not know what you are doing 


//prepare some variables 
string hexCodeQualified = "'" + calcTableName + "'[" + hexCodeColumnName + "]"; 
string transparencyQualified = "'" + calcTableName + "'[" + transparencyColumnName + "]";
int calcItemOrdinalIndex = 0; 


//required for usage of calc groups
Model.DiscourageImplicitMeasures = true;

//creates calc group 
var calculationGroupTable1 = Model.AddCalculationGroup(calculationGroupName);

//changes column name
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

//core expression of the calc item 
string calcItemExpression = 
"VAR hexValue =" + 
"\n    SELECTEDMEASURE()" + 
"\nVAR calcItemName =" + 
"\n    SELECTEDVALUE( '" + calculationGroupName + "'[" + calculationGroupColumnName +"], \"00% Transparency\" )" + 
"\nVAR hexValueOk =" + 
"\n    LEN( hexvalue ) = 7" + 
"\n        && LEFT( hexValue, 1 ) = \"#\"" + 
"\nRETURN" + 
"\n    IF(" + 
"\n        hexValueOk," + 
"\n        VAR pct =" + 
"\n            VALUE( LEFT( calcItemName, FIND( \"%\", calcItemName, 1, - 1 ) - 1 ) ) / 100" + 
"\n        VAR realPct =" + 
"\n            CALCULATE(" + 
"\n                MAX( " + transparencyQualified + " )," + 
"\n                " + transparencyQualified + " <= pct" + 
"\n            )" + 
"\n        VAR hexTransp =" + 
"\n            LOOKUPVALUE(" + 
"\n                " + hexCodeQualified + "," + 
"\n                " + transparencyQualified + ", realPct" + 
"\n            )" + 
"\n        RETURN" + 
"\n            hexValue & hexTransp" + 
"\n    )"; 


var noChangeItem = calculationGroupTable1.AddCalculationItem("No Transparency","SELECTEDMEASURE()");
calcItemOrdinalIndex++;  


foreach(var transparencyLevel in transparencyLevels){

    string calcItemName = transparencyLevel  + "% transparent"; 
    var lightCalcItem = calculationGroupTable1.AddCalculationItem(calcItemName);
    
    //use the wrapping expression around core expression 
    lightCalcItem.Expression = calcItemProtection.Replace("<CODE>",calcItemExpression); 
    lightCalcItem.FormatDax(); 
    lightCalcItem.Ordinal = calcItemOrdinalIndex; 
    calcItemOrdinalIndex++;     
};