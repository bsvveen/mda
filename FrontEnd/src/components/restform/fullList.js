import React from 'react';
import List from "./list";
import Form from "./form"
import Detail from "./detail"

const FullList = ({entityName, properties, constrains}) => { 
  const [current, setCurrent] = React.useState();
  const [mode, setMode] = React.useState("list");   
  
  const onSelect = (id, mode) => {
    console.info("onSelect", id, mode);
    setMode(mode);
    setCurrent(id);
  }   

  const onNew = () => { 
    setMode("form");
    setCurrent(undefined); 
  } 
  
  const onReset = () => { 
    setMode("list");
    setCurrent(undefined); 
  }  

  if (current && mode == "form")
    return <Form  entityName = {entityName}
                  id = {current} 
                  onCancel = {onReset}
                  onSubmit = {onReset}
                  onDelete = {onReset} />

  if (current && mode == "detail")
    return <Detail  entityName = {entityName}
                    id = {current}  
                    onCancel = {onReset} />

return (
  <div>    
    <List entityName={entityName} constrains={constrains} properties={properties} onSelect={onSelect} />
    <div className="actions"><button type="new" onClick={onNew} value="Nieuw" /></div>
  </div>
  )     
}

export default FullList;