using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

#if (XNA)
using Microsoft.Xna.Framework; 
#endif

using FarseerGames.FarseerPhysics.Mathematics;

namespace FarseerGames.FarseerPhysics.Dynamics {
    public class AngleSpring : Controller {
        protected Body body1;
        protected Body body2;

        private float springConstant;
        private float dampningConstant;
        private float targetAngle;
        private float breakpoint = float.MaxValue;
        private float maxTorque = float.MaxValue;

        private float springError;

        public AngleSpring() {}

        public AngleSpring(Body body1, Body body2, float springConstant, float dampningConstant) {
            this.body1 = body1;
            this.body2 = body2;
            this.springConstant = springConstant;
            this.dampningConstant = dampningConstant;
            this.targetAngle = this.body2.TotalRotation - this.body1.TotalRotation;
        }

        public Body Body1 {
            get { return body1; }
            set { body1 = value; }
        }

        public Body Body2 {
            get { return body2; }
            set { body2 = value; }
        }

        public float SpringConstant {
            get { return springConstant; }
            set { springConstant = value; }
        }

        public float DampningConstant {
            get { return dampningConstant; }
            set { dampningConstant = value; }
        }	

        //TODO: magic numbers
        public float TargetAngle {
            get { return targetAngle; }
            set {
                targetAngle = value;
                if (targetAngle > 5.5) { targetAngle = 5.5f; }
                if (targetAngle < -5.5f) { targetAngle = -5.5f; }
            }
        }

        public float Breakpoint {
            get { return breakpoint; }
            set { breakpoint = value; }
        }

        public float MaxTorque {
            get { return maxTorque; }
            set { maxTorque = value; }
        }

        public float SpringError {
            get { return springError; }
        }

        public override void Validate() {
            //if either of the springs connected bodies are disposed then dispose the joint.
            if (body1.IsDisposed || body2.IsDisposed) {
                Dispose();
            }
        }

        public override void Update(float dt) {
            if (Math.Abs(springError) > breakpoint) { Dispose(); } //check if joint is broken
            if (isDisposed) { return; }
            //calculate and apply spring force
            float angle = body2.totalRotation - body1.totalRotation;
            float angleDifference = body2.totalRotation - (body1.totalRotation + targetAngle);
            float springTorque = springConstant * angleDifference;
            springError = angleDifference; //keep track of 'springError' for breaking joint

            //apply torque at anchor
            if (!body1.IsStatic) {
                float torque1 = springTorque - dampningConstant * body1.angularVelocity;
                torque1 = Math.Min(Math.Abs(torque1), maxTorque) * Math.Sign(torque1);
                body1.ApplyTorque(torque1);
            }

            if (!body2.IsStatic) {
                float torque2 = -springTorque - dampningConstant * body2.angularVelocity;
                torque2 = Math.Min(Math.Abs(torque2), maxTorque) * Math.Sign(torque2);
                body2.ApplyTorque(torque2);
            }
        }
    }
}
