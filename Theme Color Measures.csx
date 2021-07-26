// '2021-05-09 / B.Agullo / added transparent color
// by Bernat Agulló
// www.esbrina-ba.com

//adapted from Darren Gosbell's script at 
// https://darren.gosbell.com/2020/08/the-best-way-to-generate-data-driven-measures-in-power-bi-using-tabular-editor/

//This script creates the color measures for each of the colors included in the theme color table. 
// See http://www.esbrina-ba.com/theme-compliant-conditional-formatting-measures/

//adjust to fit your particular model

string colorTableName = "Color"; 
string colorColumnName = "Color Name"; 
string hexCodeColumnName = "Color Code"; 

bool createTransparentColor = true; 

// do not change code below this line


string colorColumnNameWithTable = "'" + colorTableName + "'[" + colorColumnName + "]";
string hexCodeColumnNameWithTable = "'" + colorTableName + "'[" + hexCodeColumnName + "]";

string query = "EVALUATE VALUES(" + colorColumnNameWithTable + ")";
 
using (var reader = Model.Database.ExecuteReader(query))
{
    // Create a loop for every row in the resultset
    while(reader.Read())
    {
        string myColor = reader.GetValue(0).ToString();
        string measureName = myColor;
        string myExpression = "VAR HexCode = CALCULATE( SELECTEDVALUE( " + hexCodeColumnNameWithTable + "), " + colorColumnNameWithTable + " = \""  + myColor + "\") VAR Result = FORMAT(hexCode,\"@\") RETURN Result ";
                             
        var newColorMeasure = Model.Tables[colorTableName].AddMeasure(measureName, myExpression);
        newColorMeasure.FormatDax(); 
        
    }
}

if(createTransparentColor){
    var transparentMeasure = Model.Tables[colorTableName].AddMeasure("Transparent","\"#FFFFFF00\""); 
};

