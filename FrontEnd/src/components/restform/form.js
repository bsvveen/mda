import React from 'react';
import PropTypes from 'prop-types';

import Repository from './repository';
import DynamicForm from './dynamicform';

export default class Form extends React.Component {
    _isMounted = false;

    state = { contract: undefined, initial: undefined };

    PropTypes = {
        data: PropTypes.object,
        constrains: PropTypes.object,
        contract: PropTypes.object,
        controller: PropTypes.string.isRequired,
        onCancel: PropTypes.func,
        onSubmit: PropTypes.func.isRequired,
        onDelete: PropTypes.func.isRequired         
    }    

    componentDidMount() {  
        this._isMounted = true; 

        this.repository = new Repository(this.props.controller);    
        this.repository.RequestContract("Insert").then(response => { 
            if (this._isMounted) {
                this.setState({ contract: response, initial: this.getInitialFromProps() });
            }
        })
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
    
    getInitialFromProps = () => {
        if (this.props.data && this.props.data.id)
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
        if (!this.state.contract || !this.state.initial)
            return <div>Loading</div>

        const resultingContract = this.state.contract.map(i => {
            let j = this.props.contract.find(c => c.key === i.key);
            if (j) {
                return Object.keys(i).reduce((a, c) => {
                    a[c] = (j[c]) ? j[c] : i[c];
                    return a;
                }, {});   
            } 
            return i;    
        });        

        return <DynamicForm 
            initial = { this.state.initial }  
            constrains = { this.props.constrains }
            contract =  { resultingContract }
            onCancel =  { this.props.onCancel }
            onSubmit =  { this.onSubmit }
            onDelete =  { this.onDelete }           
            repository = { this.repository }                   
        />
    }
}