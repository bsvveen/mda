// https://javascript.plainenglish.io/react-component-communication-using-pubsubjs-1bec85388b1e
// https://ralexanderson.com/blog/spreading-events-in-react-with-pubsub

const topics = {}

export function subscribe(topic, fn) {
    if (!topics[topic]) topics[topic] = {}
    console.log("subscribe", topic);
    const index = topics[topic].length+1;
    topics[topic][index] = fn
    return () => {
        topics[topic][index] = null
        delete topics[topic][index]
    }
}

export function publish(topic, args) {
    if (!topics[topic]) return
    console.log("publish", topic, args);
    Object.values(topics[topic]).forEach(fn => {
        console.log("publish fn found");
        if (fn) fn(args)
    })
}