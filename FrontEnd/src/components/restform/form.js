import React from 'react';
import DynamicForm from './dynamicform';
import { useFetchById, useUpdate } from './useDataApi';
import useEntityModel from './useEntityModel';


const Form = ({entityName, id, onClose}) => {  
  const [fetchByIdResponse, setFetchByIdRequest] = useFetchById(); 
  const [updateResponse, setUpdateRequest] = useUpdate();
  const [entityModel] = useEntityModel(entityName);

  React.useEffect(() => {  
    setFetchByIdRequest(entityName, id);
  }, []); 

  console.info("Form.fetchByIdResponse", fetchByIdResponse);

  if (fetchByIdResponse.data == {}) return <p>Initializing...</p>; 
  if (fetchByIdResponse.isLoading || entityModel == undefined) return <p>Loading...</p>; 
  if (fetchByIdResponse.error) return <p>{fetchByIdResponse.error}</p>;  
  if (updateResponse.modelstate) return <p>{updateResponse.modelstate}</p>;  
  if (updateResponse.error) return <p>{updateResponse.error}</p>;  

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
        properties  = { fetchByIdResponse.data }          
        onCancel    = { onCancel }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;