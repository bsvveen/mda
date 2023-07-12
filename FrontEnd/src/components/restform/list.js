import React from 'react';
import { useFetchList } from './useDataApi';
import DynamicList from './dynamicList';

const List = ({entityName, properties, constrains, onSelect}) => {  
  const [response, setRequest] = useFetchList(entityName, properties, constrains); 

  console.info("List", response);

  if (response.isLoading) return <p>Loading...</p>; 
  if (response.error) return <p>{response.error}</p>;  

  const rowRender = (item, isSelected) => {   
    return (
      <tr className={(isSelected) ? "selected" : null} key={item.Id}> 
        {
          Object.keys(item)
          .filter((key) => { return (key !== "Id" && !key.includes("_Id"))})
          .map((key) => { return <td key={key}>{item[key]}</td>})
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