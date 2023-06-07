import React from 'react';
import useApi from './useApi';
import DynamicForm from './dynamicform';

const Form = ({entityName, id, onAction}) => {
  const {data, error, loading, fetchById, fetchList, update} = useApi({});   
  const [entityModel, setEntityModel] = React.useState(undefined);   

  React.useEffect(() => { 
    getEntityModel(entityName);
    fetchById(entityName, id);
  }, []); 

  const getEntityModel = (name) => {
    const model = JSON.parse(sessionStorage.getItem("model"));
    const entity = model.entities.find(e => e.name == name);   

    entity.properties.filter(prop => prop.type == "foreignkey").map((prop) => {
      const func = fetchList(prop.related, [prop.lookup], prop.constrains);
      return { ...prop, "func": func }      
    })

    setEntityModel(entity);
  }   

  if (loading || !entityModel) return <p>Loading...</p>;
  if (error) return <p>{error.message}</p>;  

  const onSubmit = (properties) => {   
    update(entity, id, properties);
    onAction();
  }  

  const onDelete = (id) => {    
    deleteMe(entity, id);
    onAction();
  }    

  return (    
    <div className="form">
      <DynamicForm 
        entityModel = { entityModel }  
        initialData = { data }          
        onCancel    = { onAction }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;