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

    entity.properties = entity.properties.map((prop) => {
      if(prop.type == "foreignkey") {       
        const func = fetchList.bind(null, prop.foreignkey.related, [prop.foreignkey.lookup], prop.foreignkey.constrains);
        return { ...prop, "func": func }   
      } else {
        return { ...prop }
      }   
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

  const onReset = () => {    
    if (onAction)
      onAction();
  }  

  return (    
    <div className="form">
      <DynamicForm 
        entityModel = { entityModel }  
        initialData = { data }          
        onCancel    = { onReset }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;