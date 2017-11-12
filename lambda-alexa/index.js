/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/
/**
 * This sample demonstrates a simple skill built with the Amazon Alexa Skills
 * nodejs skill development kit.
 * This sample supports multiple lauguages. (en-US, en-GB, de-DE).
 * The Intent Schema, Custom Slots and Sample Utterances for this skill, as well
 * as testing instructions are located at https://github.com/alexa/skill-sample-nodejs-fact
 **/

'use strict';

const Alexa = require('alexa-sdk');

const http = require('https');
const handlers = {
  LaunchRequest: function() {
    const that = this;
    console.log('LaunchRequest');
    http
      .get('https://hacker-api.herokuapp.com', function(res) {
        that.response.speak('HEY, SUCCESSFUL ATTACK!');
        that.emit(':responseReady');
        console.log('Got response: ' + res.statusCode);
      })
      .on('error', function(e) {
        console.log('Got error: ' + e.message);
        that.response.speak('Error to destroy machine');
        that.emit(':responseReady');
      });
  },
  Unhandled: function() {
    this.emit(':ask', 'HelpMessage', 'HelpMessage');
  },
};

exports.handler = function(event, context) {
  const alexa = Alexa.handler(event, context);

  alexa.registerHandlers(handlers);
  alexa.execute();
};
