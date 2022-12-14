import React, { Component } from 'react';
import PropTypes from 'prop-types';

import Repository from './repository';
import Form from './form';
import List from './list';

export default class ListAndNew extends Component {

  state = {    
    current: undefined,
    refresh: true
  }

  PropTypes = {    
    controller: PropTypes.string.isRequired,
    constrains: PropTypes.object,
  }

  componentDidMount() { this.repository = new Repository(this.props.controller); }    

  onFormSubmit = () => { this.setState({ current: undefined, refresh: !this.state.refresh })}

  onFormDelete = () => { this.setState({ current: undefined, refresh: !this.state.refresh })}

  onListSelect = (data) => {   
    this.repository.Get(data.id).then(response => {      
      this.setState({ current: response })
    });  
  }   

  render() {      
      return (<div className="listform">       
        <List key={this.state.refresh} controller={this.props.controller} onSelect={this.onListSelect} constrains={this.props.constrains} />         
        <hr />
        <h2 className="title">{(this.state.current) ? "Aanpassen" : "Nieuw" }</h2>
        <div>          
          <Form 
            key = { (this.state.current) ? this.state.current.id : undefined }
            data = { this.state.current }
            constrains = {this.props.constrains}
            controller = {this.props.controller}            
            onSubmit =  {this.onFormSubmit}
            onDelete = {this.onFormDelete}                   
          />
        </div>
      </div>)
    };
}