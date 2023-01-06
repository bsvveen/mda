import React from 'react';
import PropTypes from 'prop-types';

import Repository from './repository';
import DynamicForm from './dynamicform';

export default class Form extends React.Component {
    _isMounted = false;

    state = { entityModel: undefined, initialData: undefined };

    PropTypes = {        
        entityName: PropTypes.string.isRequired,
        data: PropTypes.object,        
        constrains: PropTypes.object,
        onCancel: PropTypes.func,
        onSubmit: PropTypes.func.isRequired,
        onDelete: PropTypes.func.isRequired         
    }    

    componentDidMount() {  
        this._isMounted = true; 

        this.repository = new Repository(this.props.entity);    
        this.setState({entityModel: this.getEntityModel()});   
        this.setState({initialData: this.getInitialFromProps()});  
    }

    componentWillUnmount() {
        this._isMounted = false;
    }   

    onSubmit = (properties) => {    
        const dataToSubmit = Object.assign({}, { "entity": this.props.entity, "id": this.props.id }, { "properties": properties });
        return this.repository.Submit(dataToSubmit).then(this.props.onSubmit);  
    }
    
    onDelete = (id) => {    
        return this.repository.Delete(id).then(this.props.onDelete);  
    } 

    getEntityModel = () => {
        const model = JSON.parse(sessionStorage.getItem("model"));
        const entity = model.entities.find(e => e.name == this.props.entity);

        if (!entity)
            throw console.error("requested entity not found in model", this.props.entity);

        return entity;
    } 
    
    getInitialFromProps = () => {
        console.log(this.props.data, this.props.data && this.props.data.Id);

        if (this.props.data && this.props.data.Id) 
            return this.props.data       
       
        let initial = {};
        if (this.props.constrains) {
            initial = Object.keys(this.props.constrains).filter(constrain => constrain.Operator == 0).reduce((obj, key) => {
                obj[key] = this.props.constrains[key].value;
                return obj;
            }, {});
        }

        return initial;
    } 

    render() {      
        if (!this.state.entityModel || !this.state.initialData)
            return <div>Loading</div>

        return <DynamicForm 
            entityModel =  { this.state.entityModel }  
            initialData = { this.state.initialData }  
            constrains = { this.props.constrains }           
            onCancel =  { this.props.onCancel }
            onSubmit =  { this.onSubmit }
            onDelete =  { this.onDelete }           
            repository = { this.repository }                   
        />
    }
}