#r "Microsoft.VisualBasic"
using Microsoft.VisualBasic;


if(Selected.Measures.Count <= 1) {
    Error("Select two or more measures"); 
    return; 
} 

string newMeasureName = Interaction.InputBox("New Measure name", "Name", "Sum of " + Selected.Measures.Count + " measures", 740, 400);

string newMeasureExpression = ""; 
string measureTable = ""; 

foreach(var iMeasure in Selected.Measures) { 
    if(measureTable == "") measureTable = iMeasure.Table.Name; 

    if(newMeasureExpression == "") {
        newMeasureExpression = "[" + iMeasure.Name + "]";
    } else {
        newMeasureExpression += " + [" + iMeasure.Name + "]";
    }; 
};
var newMeasure = Model.Tables[measureTable].AddMeasure(newMeasureName,newMeasureExpression); 

