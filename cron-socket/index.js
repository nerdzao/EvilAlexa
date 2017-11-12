const WebSocket = require('ws');
let connected = false;
let ws = null;
const db = require('mongojs')(
  'mongodb://campus:1234@ds243285.mlab.com:43285/campusdemo',
  ['ips', 'events'],
);
const every = require('every-moment');
function connect(ip) {
  console.log(ip);
  const ws = new WebSocket('ws://' + ip);
  return new Promise((resolve, reject) => {
    ws.on('open', function open() {
      connected = true;
      console.log('connected');
      ws.send('screen');

      return resolve(ws);
    });

    ws.on('error', function error(e) {
      console.log('test', e);
      return resolve(error);
    });
  });
}
function remove(field) {
  return new Promise((resolve, reject) => {
    db[field].remove({}, (err, res) => (err ? reject(err) : resolve(res)));
  });
}
function find(field) {
  return new Promise((resolve, reject) => {
    db[field].findOne({}, (err, res) => (err ? reject(err) : resolve(res)));
  });
}
function getIp() {
  return find('Ips');
}
async function connec2t() {
  const { ip } = await getIp();
  return await connect(ip);
}
async function exec() {
  every(1, 'second', async () => {
    const execution = await find('events');
    console.log('execution', execution);
    if (execution) {
      try {
        ws.send('screen');
      } catch (e) {
        ws = await connec2t();
      }
      await remove('events');
    }
    console.log('eee', Date.now().toLocaleString());
  });
}
exec();
