import React from 'react';
import useApi from './useApi';
import DynamicForm from './dynamicform';

const Form = ({entityName, id, onClose}) => {
  const {response, error, loading, fetchById, update} = useApi({});   
  const [entityModel, setEntityModel] = React.useState(undefined);   

  React.useEffect(() => { 
    getEntityModel(entityName);
    fetchById(entityName, id);
  }, []); 

  const getEntityModel = (name) => {
    const model = JSON.parse(sessionStorage.getItem("model"));
    if (model == undefined)
      throw Error("Model not found in sessionStorage", entity);

    const entity = model.entities.find(e => e.name == name);   
    if (entity == undefined)
      throw Error("Entity not found in Model", entity);
      
    setEntityModel(entity);
  }   

  if (loading || !entityModel) return <p>Loading...</p>;
  if (error) return <p>{JSON.stringify(error)}</p>;  

  console.info("response", response);

  const onSubmit = (properties) => {   
    update(entityName, id, properties);
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
        entityModel = { entityModel }  
        initialData = { response }          
        onCancel    = { onCancel }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;