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

  if (fetchByIdResponse.data == {}) return <p>Initializing...</p>; 
  if (fetchByIdResponse.isLoading || entityModel == undefined) return <p>Loading...</p>;  
  
  const error = fetchByIdResponse.error || updateResponse.error || createResponse.error;
  if (error) return <p>{error}</p>;   

  const validationErrors = updateResponse.modelstate || createResponse.modelstate || {};

  const succesfullFormSubmission = !error || !validationErrors;
  if (succesfullFormSubmission) {
    alert("succesfullFormSubmission");
  }

  const onSubmit = (properties) => {   
    if (id) {
      setUpdateRequest(entityName, id, properties);
    } else {
      setCreateRequest(entityName, properties);
    } 
  }  
  
  const onDelete = (id) => {    
    alert("Not Implemenyted yet")
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