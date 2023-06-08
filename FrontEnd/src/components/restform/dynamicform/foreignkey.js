import React from 'react';
import useApi from '../useApi';

const ForeignKey = ({model, value, onChange}) => {
    const {response, error, loading, fetchList} = useApi([]);  

    React.useEffect(() => {
        const FK = model.foreignkey;
        if (FK == undefined)
            throw Error("entityModel.foreignkey is undefined");

        fetchList(FK.related, [FK.lookup], FK.constrains);    
    }, []); 

    if (loading) return <p>Loading...</p>;
    if (error) return <p>{error.message}</p>;
    if (response.length === 0) return <p>Empty...</p>

    const input = response.map((l) => {
        return (
            <option 
                className="input"
                key={l.Id}
                value={l.Id}
                checked={l.Id === value}
            >{Object.values(l)[1]}</option>
        );
    });

    return (  
        <select value={value} className="input" onChange={(e) => { onChange(e, model.key) }}>
            <option value='null' key='0'>Selecteer een {model.key}</option>
            {input}
        </select>      
    );     
}

export default ForeignKey
