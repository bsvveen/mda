import React from 'react';
import PropTypes from 'prop-types';

export default class FKLookup extends React.Component {
  
    PropTypes = {       
        contract : PropTypes.object.isRequired,
        value : PropTypes.string.isRequired,
        repository : PropTypes.object.isRequired,
        constrains : PropTypes.object,        
        onChange: PropTypes.func.isRequired 
    }

    state = { searchValue: "", list: [] }      

    onChange = (e) => {        
        const display = this.props.contract.relation.targetProperties[0];
        this.setState({ searchValue: e.target.value });
        if (e.target.value.length > 3) {
            let filter = [{ "Field" : display, "operator": "Like", "value": e.target.value }]
            this.props.repository.ForeignKey(this.props.contract.key, undefined, filter).then(res => {
                this.setState({ list: res });
            }) 
        }
    }
  
    render() {  
        return (<>
            <input type="text" value={this.state.searchValue} onChange={(e) => { this.onChange(e) }} />
            <ul className="input" >
                { this.state.list.map(r => { return (
                    <div key={r.id}>
                        <a onClick={() => this.props.onChange(r.id)}>{Object.values(r)[1]}</a>                       
                    </div> 
                )})}
            </ul>
        </>);
    };
}   
 
  