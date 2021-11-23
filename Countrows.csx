foreach( Table t in Selected.Tables ) {
    t.AddMeasure(t.Name + " Rows", "COUNTROWS(" + t.DaxObjectFullName + ")" ) ; 
} 