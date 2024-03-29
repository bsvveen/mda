import React from 'react';
import { useFetchById } from './useDataApi';
import { subscribe } from "../../pubsub"
 
const Detail = ({entityName, id, onClose, subscribeTo}) => {    
  const [response, setRequest] = useFetchById(); 

  React.useEffect(() => {  
    if (id !== null)
      setRequest(entityName, id, true);

    if (subscribeTo)
        return subscribe(subscribeTo, data => { setRequest(entityName, data.id, true);})       
     
  }, []); 

  console.info("Detail", response);

  if (response.data == {}) return <p>Initializing...</p>; 
  if (response.isLoading) return <p>Loading...</p>; 
  if (response.error) return <p>{response.error}</p>;  

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
 
  return (    
    <div className="detail">    
      <div><button onClick={() => onClose()} type="close" title="Close">Close</button></div>
      {Recursive(response.data)}
    </div>
  );     
}

export default Detail;