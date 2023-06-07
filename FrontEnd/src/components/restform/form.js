import React from 'react';
import useApi from './useApi';
import DynamicForm from './dynamicform';

const Form = ({entityName, id, onAction}) => {
  const {data, error, loading, fetchById, update, deleteMe} = useApi();   
  const [entityModel, setEntityModel] = useState(undefined);   

  React.useEffect(() => {
    const getEntityModel = () => {
      const model = JSON.parse(sessionStorage.getItem("model"));
      const entity = model.entities.find(e => e.name == entityName);   
      setEntityModel(entity);
    } 

    getEntityModel();
    fetchById(entityName, id);
  }, []); 

  if (loading) return <p>Loading...</p>;
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
        onCancel    = { props.onAction() }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;