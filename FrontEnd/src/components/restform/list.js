import React from 'react';
import useApi from './useApi';
import DynamicList from './dynamicList';

const List = ({entityName, properties, constrains, onSelect}) => {
  const {response, error, loading, fetchList} = useApi();  

  React.useEffect(() => {
    fetchList(entityName, properties, constrains);
  }, []); 

  console.info("loading", loading);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error.message}</p>;  

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
      <DynamicList items={response} rowRender={rowRender}  />
    </div>
  );     
}

export default List;