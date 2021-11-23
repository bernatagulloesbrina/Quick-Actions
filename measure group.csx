#r "Microsoft.VisualBasic"
using Microsoft.VisualBasic;

// '2021-10-20 / B.Agullo / 

// Instructions:
// select the measures you want to add to a group (will act as group of headers)
// use the header in a matrix -- makes sense when you have 2 or more groups of columns
// repeat the process for any group 
// you can create 3+ layers of groups, duplicate manually the calc group 
// you need to update each of the calc items individually if you add a new measure

//
// ----- do not modify script below this line -----
//

string affectedMeasures = ""; 

if (Selected.Measures.Count == 0) {
    
    Error("No measures selected"); 
    return; 

} else if (Selected.Measures.Count != 0) {
    
    foreach(var m in Selected.Measures) {
        if(affectedMeasures == "") {
            affectedMeasures =  "[" + m.Name + "]";
        } else {
            affectedMeasures = affectedMeasures + ",[" + m.Name + "]" ;
        };
    };  
};

var ts = Model.Tables.Where(x => x.GetAnnotation("@AgulloBernat") == "Measure Group");

var timeIntelCalcGroup = null as CalculationGroupTable; 

if (ts.Count() == 1 ) {
    timeIntelCalcGroup = ts.First() as CalculationGroupTable;
} else if (ts.Count() < 1) {
    
    string calcGroupName = Interaction.InputBox("Provide a name for your 'Measure Group' Calculation Group", "Calculation Group Name", "", 740, 400);

    if(calcGroupName == "") {
        Error("No name provided");         
        return;
    };
    
    timeIntelCalcGroup = Model.AddCalculationGroup(calcGroupName);
    timeIntelCalcGroup.Description = "Under this calc group only certain measures will be visible for each calc item, see calculation items for details";
    timeIntelCalcGroup.SetAnnotation("@AgulloBernat","Measure Group");

    Model.Tables[calcGroupName].Columns["Name"].Name = calcGroupName; 

} else { 
    //this should never happen -- who needs two calc groups for time intelligence? 
    timeIntelCalcGroup = SelectTable(ts, label:"Select your 'Measure Group' Calculation Group") as CalculationGroupTable;
};

if (timeIntelCalcGroup == null) { return; } // doesn't work in TE3 as cancel button doesn't return null in TE3




string calcItemName = Interaction.InputBox("Provide a name for your Measure Group", "Measure Group Name", "", 740, 400);

if(calcItemName == "") {
    Error("No name provided");         
    return;
};

string calcItemExpression = "IF( ISSELECTEDMEASURE( " + affectedMeasures + "), SELECTEDMEASURE())";

timeIntelCalcGroup.AddCalculationItem(calcItemName,calcItemExpression); 
timeIntelCalcGroup.FormatDax(); 

CallDaxFormatter(); 

