import React from 'react';
import DynamicForm from './dynamicform';
import { useFetchById, useUpdate } from './useDataApi';
import useEntityModel from './useEntityModel';


const Form = ({entityName, id, onClose}) => {  
  const [response, setRequest] = useFetchById(); 
  const [entityModel] = useEntityModel();

  React.useEffect(() => {  
    setRequest(entityName, id);
  }, []); 

  console.info("Form", response);

  if (response.data == {}) return <p>Initializing...</p>; 
  if (response.isLoading || entityModel == undefined) return <p>Loading...</p>; 
  if (response.error) return <p>{response.error}</p>;  

  const onSubmit = (properties) => {   
    setUpdateRequest(entityName, id, properties);
    onClose();
  }  

  const onDelete = (id) => {    
    deleteMe(entityName, id);
    onClose();
  }    

  const onCancel = () => {    
    onClose();
  }  

  return (    
    <div className="form">
      <DynamicForm 
        id = { id }
        entityModel = { entityModel }  
        properties  = { getResponse.data }          
        onCancel    = { onCancel }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;