import React from 'react';
import DynamicForm from './dynamicform';
import { useFetchById, useUpdate, useCreate } from './useDataApi';
import useEntityModel from './useEntityModel';

const Form = ({entityName, id, onClose}) => {  
  const [fetchByIdResponse, setFetchByIdRequest] = useFetchById(); 
  const [updateResponse, setUpdateRequest] = useUpdate(); 
  const [createResponse, setCreateRequest] = useCreate();
  const [entityModel] = useEntityModel(entityName);

  React.useEffect(() => {  
    if (id)
      setFetchByIdRequest(entityName, id);
  }, []); 

  console.info("Form.fetchByIdResponse", fetchByIdResponse);

  if (fetchByIdResponse.data == {}) return <p>Initializing...</p>; 
  if (fetchByIdResponse.isLoading || entityModel == undefined) return <p>Loading...</p>; 
  if (fetchByIdResponse.error) return <p>{fetchByIdResponse.error}</p>;    
  if (updateResponse.error) return <p>{updateResponse.error}</p>;    
  if (createResponse.error) return <p>{createResponse.error}</p>;  

  const validationErrors = updateResponse.modelstate || createResponse.modelstate || [];

  const onSubmit = (properties) => {   
    if (id) {
      setUpdateRequest(entityName, id, properties);
    } else {
      setCreateRequest(entityName, properties);
    } 
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
        errors      = { validationErrors }          
        onCancel    = { onCancel }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;