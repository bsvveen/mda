import React from 'react';
import List from "./list";
import Form from "./form";
import Detail from "./detail";
import "./index.css";

const FullList = ({entityName, properties, constrains}) => { 
  const [current, setCurrent] = React.useState();
  const [mode, setMode] = React.useState("list");    
  const [seed, setSeed] = React.useState(1);      
  
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
    setSeed(Math.random());
  }  

  if (mode == "form")
    return <Form  entityName = {entityName}
                  id = {current} 
                  onClose = {onReset} />

  if (current && mode == "detail")
    return <Detail  entityName = {entityName}
                    id = {current}  
                    onCancel = {onReset} />

return (
  <div className="restForm">    
    <List key={seed} entityName={entityName} constrains={constrains} properties={properties} onSelect={onSelect} />
    <div className="actions"><button type="new" onClick={onNew} value="Nieuw">Nieuw</button></div>
  </div>
  )     
}

export default FullList;