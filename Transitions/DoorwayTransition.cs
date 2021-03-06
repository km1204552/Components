﻿#region File Description
//-----------------------------------------------------------------------------
// DoorwayTransition
//
// Copyright © 2014 Wave Corporation
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
#endregion

namespace WaveEngine.Components.Transitions
{
    /// <summary>
    /// Transition effect where each square of the image appears at a different time.
    /// </summary>
    public class DoorwayTransition : ScreenTransition
    {        
        /// <summary>
        /// The sprite batch
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Source transition renderTarget
        /// </summary>
        private RenderTarget sourceRenderTarget;

        /// <summary>
        /// Target transition renderTarget
        /// </summary>
        private RenderTarget targetRenderTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoorwayTransition"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public DoorwayTransition(TimeSpan duration)
            : base(duration)
        {
            this.spriteBatch = new SpriteBatch(this.graphicsDevice);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void Update(TimeSpan gameTime)
        {
            this.UpdateSources(gameTime);
            this.UpdateTarget(gameTime);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        protected override void Draw(TimeSpan gameTime)
        {
            this.sourceRenderTarget = this.graphicsDevice.RenderTargets.GetTemporalRenderTarget(
                this.platform.ScreenWidth,
                this.platform.ScreenHeight);
            this.targetRenderTarget = this.graphicsDevice.RenderTargets.GetTemporalRenderTarget(
                this.platform.ScreenWidth,
                this.platform.ScreenHeight);

            this.DrawSources(gameTime, this.sourceRenderTarget);
            this.DrawTarget(gameTime, this.targetRenderTarget);

            this.SetRenderState();
            this.graphicsDevice.RenderTargets.SetRenderTarget(null);
            this.graphicsDevice.Clear(ref this.BackgroundColor, ClearFlags.Target | ClearFlags.DepthAndStencil, 1);
            Vector2 center = new Vector2(this.sourceRenderTarget.Width / 2, this.sourceRenderTarget.Height / 2);
            float inverse = 1 - this.Lerp;

            this.spriteBatch.DrawVM(this.targetRenderTarget,
                                    center,
                                    null, 
                                    Color.White * this.Lerp, 
                                    0f, 
                                    center,
                                    new Vector2(this.Lerp), 
                                    SpriteEffects.None, 
                                    0.5f);

            int width = this.sourceRenderTarget.Width;
            int height = this.sourceRenderTarget.Height;
            int middle = width / 2;
            Rectangle rect = new Rectangle(0, 0, middle, height);

            this.spriteBatch.DrawVM(this.sourceRenderTarget,
                                    new Vector2(-middle * this.Lerp, rect.Y),
                                    rect,
                                    Color.White * inverse,
                                    0f,
                                    Vector2.Zero,
                                    Vector2.One,
                                    SpriteEffects.None,
                                    0f);

            rect.X = middle;
            this.spriteBatch.DrawVM(this.sourceRenderTarget,
                                    new Vector2(middle + (middle * this.Lerp), 0),
                                    rect,
                                    Color.White * inverse,
                                    0f,
                                    Vector2.Zero,
                                    Vector2.One,
                                    SpriteEffects.None,
                                    0f);

            this.spriteBatch.Render();

            this.graphicsDevice.RenderTargets.ReleaseTemporalRenderTarget(this.sourceRenderTarget);
            this.graphicsDevice.RenderTargets.ReleaseTemporalRenderTarget(this.targetRenderTarget);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.spriteBatch.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
