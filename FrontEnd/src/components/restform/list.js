import React from 'react';
import useApi from './useApi';
import DynamicList from './dynamicList';

const List = ({entityName, properties, constrains, onSelect}) => {
  const {response, error, loading, fetchList} = useApi();  

  React.useEffect(() => {
    fetchList(entityName, properties, constrains);
  }, []); 

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error.message}</p>;

  const rowRender = (item, isSelected) => {   
    return (
      <tr onClick={() => onSelect(item.Id, "detail")} key={item.Id} className={(isSelected) ? "selected" : null}> 
        {
          Object.keys(item)
          .filter((key) => { return (key !== "Id" && !key.includes("_Id"))})
          .map((key) => { return <td key={key}>{item[key]}</td>})
        } 
        <td className="small"><button onClick={() => onSelect(item.Id, "form")} type="edit" title="Aanpassen" /></td>               
      </tr>);
  }

  return (    
    <div className="list">
      <DynamicList items={response} rowRender={rowRender}  />
    </div>
  );     
}

export default List;