import React from 'react';

export default class SingleSelect extends React.Component {

    render () {  
        
        var { contract, value, onChange } = this.props;
        
        var input = contract.options.map((o) => {                 
            return (                
                <option key={o.key} value={o.value}>
                    {o.value}
                </option>  
            );
       });      
                   
       return (<select className="input" defaultValue={value} onChange={(e)=>{onChange(e, contract.key)}} {...contract.props}>{input}</select>);      
    }
}