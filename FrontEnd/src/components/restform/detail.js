import React from 'react';
import useApi from './useApi';

const Detail = ({entityName, id, onAction}) => {
  const {response, error, loading, fetchById} = useApi();     

  React.useEffect(() => {   
    fetchById(entityName, id);
  }, []); 

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{JSON.stringify(error)}</p>;  

  const onSubmit = (properties) => {   
    update(entity, id, properties);
    onAction();
  }  

  const onDelete = (id) => {    
    deleteMe(entity, id);
    onAction();
  }  

  const Recursive = (obj) => {
    let result = [];
    for (let key in obj) {
      (typeof obj[key] === 'object') ?          
          result.push(<div className="container" key={key}><div className="cell"> { key } </div> <div className="cell"> { Recursive(obj[key]) } </div> </div>) :  
          (obj[key] && key !== "Id" && !key.includes("_Id")) ?
            result.push(<div className="row" key={key}><div> { key } </div><div> { obj[key].toString() } </div></div>) :
            result.push()
    }     
    return result;
  }

  return (<div className="detail"> {Recursive(response)} </div>)
}

export default Detail;