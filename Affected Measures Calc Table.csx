// '2021-07-26 / B.Agullo / 
// this script creates a calculated table with the names of all selected measures to be used in calcualtion groups (and avoid typing the affected measures in each calculation item) 

//select the measures that you want to be affected by the calculation group
//before running the script. 
//measure names can also be included in the following array (no need to select them) 
string[] preSelectedMeasures = {}; //include measure names in double quotes, like: {"Profit","Total Cost"};
string affectedMeasuresTableName = "Affected Measures"; 
string affectedMeasuresColumnName = "Measure"; 




// --- do not modify below this line -- 

if (Model.Tables.Contains(affectedMeasuresTableName)){
    Error("\"" +affectedMeasuresTableName  + "\" already exists in the model"); 
    return; 
};


string affectedMeasures = "{";

int i = 0; 

for (i=0;i<preSelectedMeasures.GetLength(0);i++){
  
    if(affectedMeasures == "{") {
    affectedMeasures = affectedMeasures + "\"" + preSelectedMeasures[i] + "\"";
    }else{
        affectedMeasures = affectedMeasures + ",\"" + preSelectedMeasures[i] + "\"" ;
    }; 
    
};

if (Selected.Measures.Count != 0) {
    
    foreach(var m in Selected.Measures) {
        if(affectedMeasures == "{") {
        affectedMeasures = affectedMeasures + "\"" + m.Name + "\"";
        }else{
            affectedMeasures = affectedMeasures + ",\"" + m.Name + "\"" ;
        };
    };  
};

//check that by either method at least one measure is affected
if(affectedMeasures == "{") { 
    Error("No measures selected or preselected"); 
    return; 
};

//if there where selected or preselected measures, prepare protection code for expresion and formatstring
if(affectedMeasures != "{") { 
    
    affectedMeasures = affectedMeasures + "}";
    
    string affectedMeasureTableExpression = 
        "SELECTCOLUMNS(" + affectedMeasures + ",\"" + affectedMeasuresColumnName + "\",[Value])";

    var affectedMeasureTable = 
        Model.AddCalculatedTable(affectedMeasuresTableName,affectedMeasureTableExpression);
    
    affectedMeasureTable.FormatDax(); 
    affectedMeasureTable.Description = 
        "List of measures" ;
    
    affectedMeasureTable.IsHidden = true;     
    

};
