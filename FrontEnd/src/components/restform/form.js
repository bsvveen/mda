import React from 'react';
import PropTypes from 'prop-types';

import Repository from './repository';
import DynamicForm from './dynamicform';

export default class Form extends React.Component {
    _isMounted = false;

    state = { model: undefined, initial: undefined };

    PropTypes = {
        data: PropTypes.object,
        entity: PropTypes.string.isRequired,
        constrains: PropTypes.object,
        onCancel: PropTypes.func,
        onSubmit: PropTypes.func.isRequired,
        onDelete: PropTypes.func.isRequired         
    }    

    componentDidMount() {  
        this._isMounted = true; 

        this.repository = new Repository(this.props.entity);    
        this.setState({model: this.getDerivedModel()});   
        this.setState({initial: this.getInitialFromProps()});  
        
    }

    componentWillUnmount() {
        this._isMounted = false;
    }   

    onSubmit = (data) => {    
        return this.repository.Submit(data, data.id).then(this.props.onSubmit);  
    }
    
    onDelete = (id) => {    
        return this.repository.Delete(id).then(this.props.onDelete);  
    } 

    getDerivedModel = () => {
        const model = JSON.parse(sessionStorage.getItem("model"));
        return model.Entities.find(e => e.Name == this.props.entity).Properties;
    } 
    
    getInitialFromProps = () => {
        if (this.props.data && this.props.data.ID)
            return this.props.data
       
        let Initial = {};
        if (this.props.constrains) {
            Initial = Object.keys(this.props.constrains).filter(key => this.props.constrains[key].equals).reduce((obj, key) => {
                obj[key] = this.props.constrains[key].equals;
                return obj;
            }, {});
        }

        return Initial;
    } 

    render() {      
        if (!this.state.model || !this.state.initial)
            return <div>Loading</div>

        /*const resultingModel = this.state.model.map(i => {
            let j = this.props.model.find(c => c.key === i.key);
            if (j) {
                return Object.keys(i).reduce((a, c) => {
                    a[c] = (j[c]) ? j[c] : i[c];
                    return a;
                }, {});   
            } 
            return i;    
        });*/        

        return <DynamicForm 
            initial = { this.state.initial }  
            constrains = { this.props.constrains }
            model =  { this.state.model }
            onCancel =  { this.props.onCancel }
            onSubmit =  { this.onSubmit }
            onDelete =  { this.onDelete }           
            repository = { this.repository }                   
        />
    }
}