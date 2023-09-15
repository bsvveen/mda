import React from 'react';
import { useFetchList } from './useDataApi';
import DynamicList from './dynamicList';

import { publish } from "../../pubsub";

const List = ({entityName, properties, constrains, onSelect, publishTo}) => {  
  const [response, setRequest] = useFetchList(); 

  React.useEffect(() => {  
    setRequest(entityName, properties, constrains);
  }, []);   

  if (response.data.length == 0) return <p>Loading...</p>; 
  if (response.isLoading) return <p>Loading...</p>; 
  if (response.error) return <p>{response.error}</p>;  

  const onClick = (id) => {
    if (publishTo)
      publish(publishTo, { id: id })   
  }    

  const rowRender = (item, isSelected) => {   
    return (
      <tr className={(isSelected) ? "selected" : null} key={item.Id}> 
        {
          Object.keys(item)
          .filter((key) => { return (key !== "Id" && !key.includes("_Id"))})
          .map((key) => { return <td key={key} onClick={() => onClick(item.Id)}>{item[key]}</td>})
        } 
        <td className="small"><button onClick={() => onSelect(item.Id, "detail")} type="view" title="View">View</button></td>
        <td className="small"><button onClick={() => onSelect(item.Id, "form")} type="edit" title="Edit">Edit</button></td>               
      </tr>);
  }

  return (    
    <div className="list">    
      <DynamicList items={response.data} rowRender={rowRender}  />
    </div>
  );     
}

export default List;